using ApiTemplate.Application.Common.Interfaces;
using ApiTemplate.Application.Interfaces; // Added for DTO access
using ApiTemplate.Application.Services;
using ApiTemplate.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ApiTemplate.UnitTests.Application.Services;

public class SampleServiceTests
{
    private readonly Mock<IRepository<Sample>> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<SampleService>> _mockLogger;
    private readonly SampleService _sut;

    public SampleServiceTests()
    {
        _mockRepository = new Mock<IRepository<Sample>>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<SampleService>>();
        _sut = new SampleService(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetByIdAsync_WhenSampleExists_ReturnsSuccessResponse()
    {
        // Arrange
        var sampleId = Guid.NewGuid();
        var sample = new Sample("Test Sample", "Description");
        var sampleDto = new SampleDto(sampleId, "Test Sample", "Description", DateTime.UtcNow);

        _mockRepository.Setup(repo => repo.GetByIdAsync(sampleId, CancellationToken.None))
            .ReturnsAsync(sample);
        _mockMapper.Setup(mapper => mapper.Map<SampleDto>(sample))
            .Returns(sampleDto);

        // Act
        var result = await _sut.GetByIdAsync(sampleId, CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
        Assert.Equal(sampleDto, result.Data);
        _mockRepository.Verify(repo => repo.GetByIdAsync(sampleId, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WhenSampleDoesNotExist_ReturnsFailureResponse()
    {
        // Arrange
        var sampleId = Guid.NewGuid();
        _mockRepository.Setup(repo => repo.GetByIdAsync(sampleId, CancellationToken.None))
            .ReturnsAsync((Sample)null);

        // Act
        var result = await _sut.GetByIdAsync(sampleId, CancellationToken.None);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Null(result.Data);
        Assert.Equal("Sample not found", result.Error);
        _mockRepository.Verify(repo => repo.GetByIdAsync(sampleId, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateSample_AndReturnSuccess()
    {
        // Arrange
        var createDto = new CreateSampleDto("New Sample", "New Description");
        var expectedSampleDto = new SampleDto(Guid.NewGuid(), "New Sample", "New Description", DateTime.UtcNow);
        
        _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Sample>(), CancellationToken.None))
            .ReturnsAsync((Sample sample, CancellationToken _) => sample);
        _mockRepository.Setup(repo => repo.SaveChangesAsync(CancellationToken.None))
            .ReturnsAsync(1);
        _mockMapper.Setup(mapper => mapper.Map<SampleDto>(It.IsAny<Sample>()))
            .Returns(expectedSampleDto);

        // Act
        var result = await _sut.CreateAsync(createDto, CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
        Assert.Equal(expectedSampleDto, result.Data);
        _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Sample>(), CancellationToken.None), Times.Once);
        _mockRepository.Verify(repo => repo.SaveChangesAsync(CancellationToken.None), Times.Once);
    }
}